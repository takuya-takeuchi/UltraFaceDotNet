namespace UltraFaceDotNet
{

    /// <summary>
    /// Describes the location of a face. This class cannot be inherited.
    /// </summary>
    public sealed class FaceInfo
    {

        #region Properties

        /// <summary>
        /// Gets the x-axis value of the left side of the rectangle of face.
        /// </summary>
        public float X1
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the y-axis value of the top of the rectangle of face.
        /// </summary>
        public float Y1
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the x-axis value of the right side of the rectangle of face.
        /// </summary>
        public float X2
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the y-axis value of the bottom of the rectangle of face.
        /// </summary>
        public float Y2
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the score of the rectangle of face.
        /// </summary>
        public float Score
        {
            get;
            internal set;
        }

        #endregion

    }

}