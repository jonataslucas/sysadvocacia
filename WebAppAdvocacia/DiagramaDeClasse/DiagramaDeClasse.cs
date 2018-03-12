using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using WebAppAdvocacia.Models;


namespace WebApp_Jonatas.DiagramaDeClasse
{
    public static class DiagramaDeClasse
    {
        public static void GerarDiagrama()
        {
            using (var ctx = new ContextoEF())
            {
                using (var writer = new XmlTextWriter(@"D:\becap\programas c#\PI\Projeto de Conclusao Refeito\WebAppAdvocacia\DiagramaDeClasse\Model.edmx", Encoding.Default))
                {
                    EdmxWriter.WriteEdmx(ctx, writer);
                }
            }
        }
    }
}