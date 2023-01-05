namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// The job run artifact dto.
    /// </summary>
    public class JobRunArtefactDto
    {
        /// <summary>
        /// Filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Artifact size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; }
    }
}