using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionMonitor
{
    public class HistorialDato
    {
        public string fecha { get; set; }
        public string ambiente { get; set; }
        public string imagen_base64 { get; set; }
        public int sad { get; set; }  // O las emociones correctas que estés usando
    }

}
