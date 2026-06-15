using BuildingBlocks.Exceptions;

namespace Patients.Application.Exceptions;

public class PortraitNotFoundException(string ownerType)
    : NotFoundException($"{ownerType} portrait was not found.");
