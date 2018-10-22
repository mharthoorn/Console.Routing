namespace Shell.Routing
{
    /* 

        -- the order of flags should not matter
        -- flags are: --name or -n
        -- flags are disambiguated by testing.
        -- option values are: name=value
        -- format=xml

        fhir save --all
        fhir save -a

        fhir install --here hl7.fhir.core.stu3 

        fhir 



    

    */

    public interface IArgument
    {
        bool Match(string name);
    }


}