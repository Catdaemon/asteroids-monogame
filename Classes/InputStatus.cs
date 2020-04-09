namespace monogame_test
{
    public class InputStatus
    {
        public bool Forward { get; set; }
        public bool Backward { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public float ForwardAmount { get; set; } = 1;
        public float BackwardAmount { get; set; } = 1;
        public float LeftAmount { get; set; } = 1;
        public float RightAmount { get; set; } = 1;
    }
}