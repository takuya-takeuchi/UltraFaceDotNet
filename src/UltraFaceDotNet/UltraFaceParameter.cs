namespace UltraFaceDotNet
{

    public sealed class UltraFaceParameter
    {

        #region Properties

        public string BinFilePath
        {
            get;
            set;
        }

        public string ParamFilePath
        {
            get;
            set;
        }

        public int InputWidth
        {
            get;
            set;
        }

        public int InputLength
        {
            get;
            set;
        }

        public int NumThread
        {
            get;
            set;
        } = 4;

        public float ScoreThreshold
        {
            get;
            set;
        } = 0.7f;

        public float IouThreshold
        {
            get;
            set;
        } = 0.3f;

        public int TopK
        {
            get;
            set;
        } = -1;

        #endregion

    }

}