namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string? Name{ get; set; }
        public double? Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation property
        public virtual Region? Region { get; set; }
        public virtual WalkDifficulty? WalkDifficulty { get; set; }

    }
}
