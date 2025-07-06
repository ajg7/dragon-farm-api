using System.ComponentModel.DataAnnotations;

namespace DragonFarmApi.DTOs
{
    public class DragonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Age { get; set; }
        public double Weight { get; set; }
        public bool IsHealthy { get; set; }
        public DateTime DateAcquired { get; set; }
        public string? Description { get; set; }
    }

    public class CreateDragonDto
    {
        [Required(ErrorMessage = "Dragon name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Species is required")]
        [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters")]
        public string Species { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color is required")]
        [StringLength(30, ErrorMessage = "Color cannot exceed 30 characters")]
        public string Color { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Age must be between 0 and 10000")]
        public int Age { get; set; }

        [Range(0.1, 100000, ErrorMessage = "Weight must be between 0.1 and 100000")]
        public double Weight { get; set; }

        public bool IsHealthy { get; set; } = true;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class UpdateDragonDto
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters")]
        public string? Species { get; set; }

        [StringLength(30, ErrorMessage = "Color cannot exceed 30 characters")]
        public string? Color { get; set; }

        [Range(0, 10000, ErrorMessage = "Age must be between 0 and 10000")]
        public int? Age { get; set; }

        [Range(0.1, 100000, ErrorMessage = "Weight must be between 0.1 and 100000")]
        public double? Weight { get; set; }

        public bool? IsHealthy { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}