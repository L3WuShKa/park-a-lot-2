namespace Administrator
{
    public interface IAdminAccess
    {
        bool Authenticate(string password);
        void ShowAdminMenu();
        void AfiseazaLogSecuritate();
        void StergeLogSecuritate();
        void AfiseazaMasiniInParcare();
        void AfiseazaIstoricPlati();
        void CautaMasina(string nrInmatriculare);
    }
}
