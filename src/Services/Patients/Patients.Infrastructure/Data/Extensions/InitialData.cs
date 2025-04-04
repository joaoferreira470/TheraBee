using Patients.Domain.Models;
using Patients.Domain.ValueObjects;

namespace Patients.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Patient> Patients
    {
        get
        {
            var address1 = Address.Of("Rua Agolada n50", "Santarém", "Vale Mansos", "2100-049");
            var address2 = Address.Of("Rua 25 de Abril", "Setúbal", "Paivas", "2845-389");

            var patient1 = Patient.Create(
                                    PatientId.Of(Guid.NewGuid()),
                                    "João",
                                    DateTime.SpecifyKind(new DateTime(1992, 3, 14), DateTimeKind.Utc),
                                    patientAddress: address1,
                                    "Sofre de pouca de extrema bossidão",
                                    "Extremamente lindo em geral",
                                    Guid.Parse("F372455A-C80C-487C-85A6-C5C6F18A83A8")
                                    );

            var patient2 = Patient.Create(
                                    PatientId.Of(Guid.NewGuid()),
                                    "Neuza",
                                    DateTime.SpecifyKind(new DateTime(1993, 3, 26), DateTimeKind.Utc),
                                    patientAddress: address2,
                                    "Sofre de incapacidade de parar de trabalhar",
                                    "Pinguina super linda",
                                    Guid.Parse("F372455A-C80C-487C-85A6-C5C6F18A83A8")
                                    );

            return new List<Patient> { patient1, patient2 };
        }
    }
}
