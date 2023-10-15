using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fnxpe002.Model
{
    public class Pedido
    {
        public Pedido()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; private set; }
        public DateTime DataPedido { get; set; }
        public string Item { get; set; }
        public int Quantidade { get; set; }
        public float Valor  { get; set; }
    }
}
