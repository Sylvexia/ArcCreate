using MoonSharp.Interpreter;

namespace ArcCreate.Gameplay.Scenecontrol
{
    [MoonSharpUserData]
    public abstract class ValueChannel : SerializableUnit
    {
        public static ValueChannel ConstantZeroChannel { get; } = new ConstantChannel(0);

        public static ValueChannel ConstantOneChannel { get; } = new ConstantChannel(1);

        public static ValueChannel operator +(ValueChannel a) => a;

        public static NegateChannel operator -(ValueChannel a) => new NegateChannel(a);

        public static SumChannel operator +(ValueChannel a, ValueChannel b) => new SumChannel(a, b);

        public static SumChannel operator +(ValueChannel a, float b) => a + new ConstantChannel(b);

        public static SumChannel operator +(float b, ValueChannel a) => a + new ConstantChannel(b);

        public static SumChannel operator -(ValueChannel a, ValueChannel b) => new SumChannel(a, -b);

        public static SumChannel operator -(ValueChannel a, float b) => a + new ConstantChannel(-b);

        public static SumChannel operator -(float b, ValueChannel a) => -a + new ConstantChannel(b);

        public static ProductChannel operator *(ValueChannel a, float b) => a * new ConstantChannel(b);

        public static ProductChannel operator *(float b, ValueChannel a) => a * new ConstantChannel(b);

        public static ProductChannel operator *(ValueChannel a, ValueChannel b) => new ProductChannel(a, b);

        public static ProductChannel operator /(ValueChannel a, float b) => a * new ConstantChannel(1 / b);

        public static ProductChannel operator /(float b, ValueChannel a) => new ConstantChannel(b) * new InverseChannel(a);

        public static ProductChannel operator /(ValueChannel a, ValueChannel b) => a * new InverseChannel(b);

        public abstract float ValueAt(int timing);
    }
}