using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrudLinQWPF
{

    
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["CrudLinQWPF.Properties.Settings.CrudLinqSql"].ConnectionString;
            dataContext = new DataClasses1DataContext(miConexion);

            //InsertaEmpresa();
            //InsertaEmpleado();
            //InsertaCargos();
            //InsertaCargoEmpleado();
            //ActualizarDatos();
            //ActualizaEmpleado();
            EliminaEmpleado();
        }

        public void InsertaEmpresa()
        {
            //dataContext.ExecuteCommand("delete from Empresa");
            
            Empresa pildorasInformaticas = new Empresa();

            pildorasInformaticas.Nombre = "Pildoras Informáticas";
            
            Empresa empresaGoogle = new Empresa();

            empresaGoogle.Nombre = "Google";

            dataContext.Empresa.InsertOnSubmit(pildorasInformaticas);
            dataContext.Empresa.InsertOnSubmit(empresaGoogle);

            dataContext.SubmitChanges();

            DGVPrincipal.ItemsSource = dataContext.Empresa;
        }

        public void InsertaEmpleado()
        {
            Empresa empPildorasInformaticas = dataContext.Empresa.First(em => em.Nombre.Equals("Pildoras Informáticas"));
            Empresa empGoogle = dataContext.Empresa.First(em => em.Nombre.Equals("Google"));

            List<Empleado> listaEmpleados = new List<Empleado>();
            listaEmpleados.Add(new Empleado { Nombre="Juan", Apellido="Diaz", EmpresaId=empPildorasInformaticas.Id});
            listaEmpleados.Add(new Empleado { Nombre="Ana", Apellido="Martín", EmpresaId=empGoogle.Id});
            listaEmpleados.Add(new Empleado { Nombre="María", Apellido="López", EmpresaId=empGoogle.Id});
            listaEmpleados.Add(new Empleado { Nombre="Antonio", Apellido="Fernández", EmpresaId=empPildorasInformaticas.Id});

            dataContext.Empleado.InsertAllOnSubmit(listaEmpleados);
            dataContext.SubmitChanges();
            DGVPrincipal.ItemsSource = dataContext.Empleado;
        }

        public void InsertaCargos()
        {
            //dataContext.ExecuteCommand("delete from Cargo");
            List<Cargo> cargos= new List<Cargo> 
            {
                new Cargo { NombreCargo = "Administrativo/a" },
                new Cargo { NombreCargo = "Director/a" },
                new Cargo { NombreCargo = "Empleado/a" }
            };

            dataContext.Cargo.InsertAllOnSubmit(cargos);
            dataContext.SubmitChanges();

            DGVPrincipal.ItemsSource = dataContext.Cargo;
        }

        public void InsertaCargoEmpleado()
        {
            //dataContext.ExecuteCommand("Delete from CargoEmpleado");
            Cargo cargoDirector = dataContext.Cargo.First(C => C.NombreCargo.Equals("Director/a"));
            Cargo cargoAdministrativo = dataContext.Cargo.First(C => C.NombreCargo.Equals("Administrativo/a"));
            Cargo cargoEmpleado = dataContext.Cargo.First(C => C.NombreCargo.Equals("Empleado/a"));

            Empleado Juan = dataContext.Empleado.First(E => E.Nombre.Equals("Juan"));
            Empleado Ana = dataContext.Empleado.First(E => E.Nombre.Equals("Ana"));
            Empleado Maria = dataContext.Empleado.First(E => E.Nombre.Equals("María"));
            Empleado Antonio = dataContext.Empleado.First(E => E.Nombre.Equals("Antonio"));


            List<CargoEmpleado> cargosEmpleados = new List<CargoEmpleado>
            {
                new CargoEmpleado { EmpleadoId = Juan.Id ,CargoId = cargoDirector.Id },
                new CargoEmpleado { EmpleadoId = Ana.Id ,CargoId = cargoAdministrativo.Id },
                new CargoEmpleado { EmpleadoId = Maria.Id ,CargoId = cargoEmpleado.Id },
                new CargoEmpleado { EmpleadoId = Antonio.Id ,CargoId = cargoDirector.Id }
            };

            dataContext.CargoEmpleado.InsertAllOnSubmit(cargosEmpleados);
            dataContext.SubmitChanges();

            DGVPrincipal.ItemsSource = dataContext.CargoEmpleado;
        }

        public void ActualizarDatos()
        {

            IEnumerable<CargoEmpleado> query = from cargoEmpleado in dataContext.CargoEmpleado
                        where cargoEmpleado.Id == 17 select cargoEmpleado;

            foreach (CargoEmpleado item in query)
            {
                item.CargoId = 8;
            }

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            DGVPrincipal.ItemsSource = dataContext.CargoEmpleado;

        }

        public void ActualizaEmpleado()
        {
            Empleado Maria = dataContext.Empleado.First(E => E.Nombre.Equals("María"));

            Maria.Nombre = "María Angeles";

            dataContext.SubmitChanges();
            DGVPrincipal.ItemsSource = dataContext.Empleado;
        }

        public void EliminaEmpleado()
        {
            Empleado Juan = dataContext.Empleado.First(E => E.Nombre.Equals("Juan"));

            dataContext.Empleado.DeleteOnSubmit(Juan);
            dataContext.SubmitChanges();
            DGVPrincipal.ItemsSource = dataContext.Empleado;
        }
    }

   
}
