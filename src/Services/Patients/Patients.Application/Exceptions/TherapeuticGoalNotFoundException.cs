using BuildingBlocks.Exceptions;

namespace Patients.Application.Exceptions;

public class TherapeuticGoalNotFoundException : NotFoundException
{
    public TherapeuticGoalNotFoundException(Guid id) : base("TherapeuticGoal", id)
    {
    }
}
