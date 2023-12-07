namespace EntreEspeciesNuevo.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string NuevaContraseña { get; set; }
        public string ConfirmarContraseña { get; set; }
    }
}
