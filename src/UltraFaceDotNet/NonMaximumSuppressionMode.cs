namespace UltraFaceDotNet
{

    /// <summary>
    /// Specifies the mode of Non Maximum Suppression.
    /// </summary>
    public enum NonMaximumSuppressionMode
    {

        /// <summary>
        /// Specifies that the hard nms.
        /// </summary>
        Hard = 1,

        /// <summary>
        /// Specifies that the blending nms. The mix nms was been proposed in paper blaze face, aims to minimize the temporal jitter. 
        /// </summary>
        Blending = 2

    }

}