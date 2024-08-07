using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace MangaUpdater.IntegrationTests.Setup;

public class NoCircularReferencesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(behavior => fixture.Behaviors.Remove(behavior));
    }
}

public class IgnoreVirtualMembersCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new IgnoreVirtualMembers());
    }
}

public class IgnoreVirtualMembers : ISpecimenBuilder
{
    public object? Create(object request, ISpecimenContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var propertyInfo = request as PropertyInfo;
        if (propertyInfo == null)
        {
            return new NoSpecimen();
        }

        if (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsVirtual)
        {
            return null;
        }

        return new NoSpecimen();
    }
}

public class NoIdSpecimenCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new NoIdSpecimenBuilder());
    }
}

public class NoIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var propertyInfo = request as PropertyInfo;
        
        if (propertyInfo != null && propertyInfo.Name.Equals("Id"))
        {
            return new OmitSpecimen();
        }

        var fieldInfo = request as FieldInfo;
        if (fieldInfo != null && fieldInfo.Name.Equals("Id"))
        {
            return new OmitSpecimen();
        }

        return new NoSpecimen();
    }
}