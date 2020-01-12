namespace UltraFaceDotNet
{

    /// <summary>
    /// Represents a parameter of UltraFace. This class cannot be inherited.
    /// </summary>
    public sealed class UltraFaceParameter
    {

        #region Properties

        /// <summary>
        /// Gets or sets the file path of the model binary file.
        /// </summary>
        public string BinFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file path of the param file.
        /// </summary>
        public string ParamFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pixel width after resized input image.
        /// </summary>
        public int InputWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pixel height after resized input image.
        /// </summary>
        public int InputLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the count of threads for processing of neural network. The default is 4.
        /// </summary>
        public int NumThread
        {
            get;
            set;
        } = 4;

        /// <summary>
        /// Gets or sets the score threshold for detecting face. The default is 0.7f.
        /// </summary>
        public float ScoreThreshold
        {
            get;
            set;
        } = 0.7f;

        /// <summary>
        /// Gets or sets the IoU (Intersection Of Union) threshold for detecting face. The default is 0.3f.
        /// </summary>
        public float IouThreshold
        {
            get;
            set;
        } = 0.3f;

        /// <summary>
        /// Gets or sets the upper limit of detecting face. The default is -1.
        /// </summary>
        public int TopK
        {
            get;
            set;
        } = -1;

        #endregion

    }

}