using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Gestao_veiculos.Validators
{
    public sealed partial class CpfCnpjAttribute : ValidationAttribute
    {
        public CpfCnpjAttribute() : base("CPF ou CNPJ inválido.") { }

        public override bool IsValid(object? value)
        {
            if (value is not string raw) return false;
            var d = DigitsOnly().Replace(raw, "");
            return d.Length == 11 ? IsValidCpf(d) : d.Length == 14 && IsValidCnpj(d);
        }

        private static bool IsValidCpf(string d)
        {
            if (new string(d[0], 11) == d) return false;

            int sum = 0;
            for (int i = 0; i < 9; i++) sum += (d[i] - '0') * (10 - i);
            int rem = sum % 11;
            if ((rem < 2 ? 0 : 11 - rem) != (d[9] - '0')) return false;

            sum = 0;
            for (int i = 0; i < 10; i++) sum += (d[i] - '0') * (11 - i);
            rem = sum % 11;
            return (rem < 2 ? 0 : 11 - rem) == (d[10] - '0');
        }

        private static bool IsValidCnpj(string d)
        {
            if (new string(d[0], 14) == d) return false;

            int[] w1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int sum = 0;
            for (int i = 0; i < 12; i++) sum += (d[i] - '0') * w1[i];
            int rem = sum % 11;
            if ((rem < 2 ? 0 : 11 - rem) != (d[12] - '0')) return false;

            int[] w2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            sum = 0;
            for (int i = 0; i < 13; i++) sum += (d[i] - '0') * w2[i];
            rem = sum % 11;
            return (rem < 2 ? 0 : 11 - rem) == (d[13] - '0');
        }

        [GeneratedRegex(@"\D")]
        private static partial Regex DigitsOnly();
    }
}
