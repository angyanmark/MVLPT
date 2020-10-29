using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace ImageStore.Backend.Web.Extensions
{
    public static class OptionsExtensions
    {
        /// <summary>
        /// Megadott típusú options osztályt beregisztrál későbbi DI használatra, és visszaadja az options értékét a Startup.ConfigureServices metódusban való használathoz
        /// </summary>
        /// <typeparam name="TOption">options osztály típusa</typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns><typeparamref name="TOption"/> példány, feltöltve a konfigurációnak megfelelően</returns>
        public static TOption ConfigureOption<TOption>(this IServiceCollection services, IConfiguration configuration, IValidator<TOption> validator = null)
            where TOption : class, new()
        {
            var options = new TOption();

            // aktuális metódusban való használathoz
            configuration.GetSection(typeof(TOption).Name).Bind(options);
            // IOption<TOption> alapú DI használathoz
            services.AddOptions<TOption>(typeof(TOption).Name);
            services.Configure<TOption>(configuration.GetSection(typeof(TOption).Name));

            if (validator != null)
            {
                var validationResult = validator.Validate(options);
                if (!validationResult.IsValid)
                {
                    throw new OptionsValidationException(typeof(TOption).Name, typeof(TOption),
                        validationResult.Errors.Select(e => e.ErrorMessage));
                }
            }

            return options;
        }
    }
}
