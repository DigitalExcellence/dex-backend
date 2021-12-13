using System.ComponentModel.DataAnnotations;

namespace API.InputOutput.ActivityAlgorithm
{
    /// <summary>
    /// The class for the ActivityAlgorithmInput
    /// </summary>
    public class ActivityAlgorithmInput
    {
        /// <summary>
        /// Multiplier for the LikeDataField
        /// </summary>
        [Required]
        public int LikeDataMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the RecentCreatedDataField
        /// </summary>
        [Required]
        public int RecentCreatedDataMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the AverageLikeDateField
        /// </summary>
        [Required]
        public int AverageLikeDateMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the UpdatedTimeField
        /// </summary>
        [Required]
        public int UpdatedTimeMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the InstitutionField
        /// </summary>
        [Required]
        public int InstitutionMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the ConnectedCollaboratorsField
        /// </summary>
        [Required]
        public int ConnectedCollaboratorsMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the MetaDataField
        /// </summary>
        [Required]
        public int MetaDataMultiplier { get; set; }
        /// <summary>
        /// Multiplier for the RepoScoreField
        /// </summary>
        [Required]
        public int RepoScoreMultiplier { get; set; }
    }
}
