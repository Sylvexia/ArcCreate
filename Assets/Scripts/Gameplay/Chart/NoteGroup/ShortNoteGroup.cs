using ArcCreate.Gameplay.Data;
using ArcCreate.Utility.Extension;
using UnityEngine;

namespace ArcCreate.Gameplay.Chart
{
    /// <summary>
    /// Base class for taps and arc taps note groups.
    /// </summary>
    /// <typeparam name="Note">The note type.</typeparam>
    /// <typeparam name="Behaviour">The MonoBehaviour component type corresponding to the note type.</typeparam>
    public abstract class ShortNoteGroup<Note, Behaviour> : NoteGroup<Note, Behaviour>
        where Note : INote<Behaviour>
        where Behaviour : MonoBehaviour
    {
        private CachedBisect<Note, int> timingSearch;
        private CachedBisect<Note, double> floorPositionSearch;
        private int lastRenderRangeLower = int.MaxValue;
        private int lastRenderRangeUpper = int.MaxValue - 1;

        public override int ComboAt(int timing)
        {
            return timingSearch.List.BisectRight(timing, note => note.Timing);
        }

        public override void Update(int timing, double floorPosition, GroupProperties groupProperties)
        {
            if (Notes.Count == 0)
            {
                return;
            }

            UpdateJudgement(timing, groupProperties);
            UpdateRender(timing, floorPosition, groupProperties);
        }

        public override void RebuildList()
        {
            timingSearch = new CachedBisect<Note, int>(Notes, note => note.Timing);
            floorPositionSearch = new CachedBisect<Note, double>(Notes, note => note.FloorPosition);
        }

        private void UpdateJudgement(int timing, GroupProperties groupProperties)
        {
            if (groupProperties.NoInput)
            {
                return;
            }

            int judgeFrom = timing - Values.LostJudgeWindow;
            int judgeTo = timing + Values.LostJudgeWindow;
            int judgeIndex = timingSearch.Bisect(judgeFrom);
            while (judgeIndex < timingSearch.List.Count)
            {
                Note note = timingSearch.List[judgeIndex];
                if (note.Timing > judgeTo)
                {
                    break;
                }

                timingSearch.List[judgeIndex].UpdateJudgement(timing, groupProperties);
                judgeIndex++;
            }
        }

        private void UpdateRender(int timing, double floorPosition, GroupProperties groupProperties)
        {
            double fpDistForward = System.Math.Abs(ArcFormula.ZToFloorPosition(Values.TrackLengthForward));
            double fpDistBackward = System.Math.Abs(ArcFormula.ZToFloorPosition(Values.TrackLengthBackward));
            double renderFrom =
                (groupProperties.NoInput && !groupProperties.NoClip) ?
                floorPosition :
                floorPosition - fpDistBackward;
            double renderTo = floorPosition + fpDistForward;

            int renderIndex = floorPositionSearch.Bisect(renderFrom);

            // Disable old notes
            for (int i = lastRenderRangeLower; i <= lastRenderRangeUpper; i++)
            {
                Note note = floorPositionSearch.List[i];

                if (note.FloorPosition < renderFrom || note.FloorPosition > renderTo)
                {
                    Pool.Return(note.RevokeInstance());
                }
            }

            lastRenderRangeLower = renderIndex;

            // Update notes
            while (renderIndex < floorPositionSearch.List.Count)
            {
                Note note = floorPositionSearch.List[renderIndex];
                if (note.FloorPosition > renderTo)
                {
                    break;
                }

                if (!note.IsAssignedInstance)
                {
                    note.AssignInstance(Pool.Get(ParentTransform));
                }

                note.UpdateInstance(timing, floorPosition, groupProperties);
                renderIndex++;
            }

            lastRenderRangeUpper = Mathf.Min(renderIndex, floorPositionSearch.List.Count - 1);
        }
    }
}