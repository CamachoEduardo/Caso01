using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caso01
{
    public partial class frmLlamdas : Form
    {
        //Declaración de variables GLOBALES
        string tipo;
        string horario;
        int minutos;
        double costoMinuto;
        double costoLlamada;
        double mayorMonto;
        string horarioMayor;
        string tipoMayor;
        //Contadores y acumuladores 
        int cLlamadas;
        double aLocNaC, aLocInt, aMovNac, aMovInt;

        private void tHora_Tick(object sender, EventArgs e)
        {
            //Mostrar la hora
            lblHora.Text = DateTime.Now.ToString("hh:mn:ss");

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //Capturando los datos
            horario = cboHorario.Text;
            minutos = int.Parse(txtMinutos.Text);
            //Determinar el costo por minuto
            asignaCostoxMinuto();
            //Determinar el costo por llamada
            asignaCostoxLlamada();
            //Imprimir el registro de llamadas
            imprimirRegistro();
            lvEstadisticas.Items.Clear();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Enviar los datos a la lista de estadísticas 
            imprimirEstadisticas();

        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Asignar el costo por minuto 
            asignaCostoxMinuto();
            lblCosto.Text = costoMinuto.ToString("C");

        }

        private void frmLlamdas_Load(object sender, EventArgs e)
        {
            //Mostrar la fecha actual
            lblFecha.Text = DateTime.Now.ToShortDateString();

        }

        public frmLlamdas()
        {
            InitializeComponent();
            tHora.Enabled = true;
        }
        //Método que permite asignar el costo por minuto según el tipo de llamada 
        void asignaCostoxMinuto()
        {
            //Capturando el tipo de llamada desde el cuadro combinado 
            tipo = cboTipo.Text;
            //Asignando el costo por minuto según el tipo de llamada 
            switch (tipo)
            {
                case "Local Nacional": costoMinuto = 0.20; break;
                case "Local Internacional": costoMinuto = 0.50; break;
                case "Movil Nacional": costoMinuto = 1.20; break;
                case "Movil Internacional": costoMinuto = 2.20; break;
            }
        }
        //Método que permite asignar por llamada según el horario 
        void asignaCostoxLlamada()
        {
            //Variables locales
            double importe = costoMinuto * minutos;
            double descuento = 0;
            //Asignando el descuento según el horario 
            switch (horario)
            {
                case "Diurno (07:00-13:00)": descuento = importe * 0.3; break;
                case "Tarde (13:00-19:00)": descuento = importe * 0.2; break;
                case "Noche (19:00-23:00)": descuento = importe * 0.1; break;
                case "Madrugada (23:00-07:00)": descuento = importe * 0.3; break;
            }
            costoLlamada = importe - descuento;
        }
        //Método que permite imprimir los valores en la lista de registro 
        void imprimirRegistro()
        {
            ListViewItem fila = new ListViewItem(tipo);
            fila.SubItems.Add(horario);
            fila.SubItems.Add(minutos.ToString());
            fila.SubItems.Add(costoMinuto.ToString("0.00"));
            fila.SubItems.Add(costoLlamada.ToString("0.00"));
            lvRegistro.Items.Add(fila);
        }
        //Método que permite mostrar los valores GL0BALES para la estadística 
        void imprimirEstadisticas()
        {
            //Contar el número de llamadas entre 10 y 30 minutos 
            numeroLlamadas();
            //Monto acumulado del costo por llamada por tipo 
            costoAcumuladoxtipo();
            //Mayor costo por llamada, que tipo y horario 
            mayorMontoLlamada();
            //Enviando los resultados a la lista de Estadísticas
            lvEstadisticas.Items.Clear();
            string[] elementosFila = new string[2];
            ListViewItem row;
            elementosFila[0] = "Número de llamadas entre 10 y 30 minutos";
            elementosFila[1] = cLlamadas.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Local Nacional";
            elementosFila[1] = aLocNaC.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Local Internacional";
            elementosFila[1] = aLocInt.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Móvil Nacional";
            elementosFila[1] = aMovNac.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Costo acumulado por Móvil Internacional";
            elementosFila[1] = aMovInt.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Mayor monto de llamada";
            elementosFila[1] = mayorMonto.ToString();
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Tipo de llamada con mayor monto";
            elementosFila[1] = tipoMayor;
            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);

            elementosFila[0] = "Horario con mayor monto";
            elementosFila[1] = horarioMayor;

            row = new ListViewItem(elementosFila);
            lvEstadisticas.Items.Add(row);
        }
        //Determinar el número de llamadas entre 10 y 30 minutos 
        void numeroLlamadas()
        {
            //Inicializar el contador de llamadas 
            cLlamadas = 0;
            //Recorremos por todos los registros de la lista 
            for (int i = 0; i < lvRegistro.Items.Count; i++)
            {
                //Capturamos los minutos
                int minutos = int.Parse(lvRegistro.Items[i].SubItems[2].Text);
                //Comparamos si los minutos se encuentran en el rango de 10 y 30 
                if (minutos >= 10 && minutos <= 30) cLlamadas++;
            }
        }
        //Método que determina el total acumulado del costo por llamada 
        void costoAcumuladoxtipo()
        {
            //Inicializar las variables acumuladoras en cero 
            aLocNaC = 0; aLocInt = 0; aMovNac = 0; aMovInt = 0;
            //Recorriendo por todos los registros
            for (int i = 0; i < lvRegistro.Items.Count; i++)
            {
                //Capturando el tipo de llamada
                string t = lvRegistro.Items[i].SubItems[0].Text;
                //Condicionar el tipo de llamadas para realizar la acumulacion 
                if (t == "Local Nacional")
                    aLocNaC += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Local Internacional")
                    aLocInt += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Movil Nacional")
                    aMovNac += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                else if (t == "Movil Internacional")
                    aMovInt += double.Parse(lvRegistro.Items[i].SubItems[4].Text);
            }
        }
        //Metodo que determina el mayor monto de llamada 
        void mayorMontoLlamada()
        {
            //Inicializar la variable local posicion 
            int posicion = 0;
            //Inicializar la variable mayor con el primer costo de los registros 
            mayorMonto = double.Parse(lvRegistro.Items[0].SubItems[4].Text);
            //Recorrer por todos los registros
            for (int i = 0; i < lvRegistro.Items.Count; i++)
            {
                //Si uno de los costos es mayor que el valor asignado a la variable 
                //mayor entonces hemos encontrado el mayor de los elementos 
                if (double.Parse(lvRegistro.Items[i].SubItems[4].Text) > mayorMonto)
                {
                    mayorMonto = double.Parse(lvRegistro.Items[i].SubItems[4].Text);
                    posicion = i;
                }
            }
            tipoMayor = lvRegistro.Items[posicion].SubItems[0].Text;
            horarioMayor = lvRegistro.Items[posicion].SubItems[1].Text;
        }
    }
}








    

