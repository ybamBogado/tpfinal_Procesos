using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using tp1;

namespace tpfinal
{
    public class Estrategia
    {
        private Proceso[] heap; // La lista que simula la MinHeap
        private Proceso[] maxHeap; // La lista que simula la MaxHeap
        private int elementos;
        public String Consulta1(List<Proceso> datos)
        {
            StringBuilder result = new StringBuilder();
            List<Proceso> list = new List<Proceso>();

            if (heap == null)
            {
                // Por si no se ejecutó para tener heap con datos
                ShortesJobFirst(datos, list);
            }

            // Calcular la cantidad de elementos y el primer índice de hojas para MinHeap
            int elementosMinHeap = heap.Length;
            //como la heap se ordena por niveles si sabemos la posicion donde comienza
            //el ultimo nivel, imprimiendo desde ahi tendriamos las hojas
            int primerIndiceHojaMinHeap = elementosMinHeap / 2;

            result.AppendLine("Hojas de la MinHeap (basadas en tiempo):");
            for (int i = primerIndiceHojaMinHeap; i < elementosMinHeap; i++)
            {
                // Acceder a los procesos en el array heap
                Proceso hoja = heap[i];
                result.AppendLine("Proceso: " + hoja.ToString());
            }

            if (maxHeap == null)
            {
                // Por si no se ejecutó para tener maxHeap con datos
                PreemptivePriority(datos, list);
            }

            // Calcular la cantidad de elementos y el primer índice de hojas para MaxHeap
            int elementosMaxHeap = maxHeap.Length;
            int primerIndiceHojaMaxHeap = elementosMaxHeap / 2;

            result.AppendLine("\nHojas de la MaxHeap (basadas en prioridad):");
            for (int i = primerIndiceHojaMaxHeap; i < elementosMaxHeap; i++)
            {
                // Acceder a los procesos en el array maxHeap
                Proceso hoja = maxHeap[i];
            
                result.AppendLine("Proceso: " + hoja.ToString());
            }

            return result.ToString();
        }


        public String Consulta2(List<Proceso> datos)
        {
            //formula para sacar altura
            //ℎ=[log^2(𝑛)]
            //n= cantidad de nodos
            int n = datos.Count();
            //ambas tienen la misma cantidad de nodos,
            //por lo que el resultado es el mismo para ambas
            int logaritmo= (int)Math.Floor(Math.Log(n) / Math.Log(2));
            //resultado del logaritmo pasado a string
            string resutl = logaritmo.ToString();
            return "Las heaps poseen: " +  resutl + " niveles";
        }
        public String Consulta3(List<Proceso> datos)
        {
            StringBuilder result = new StringBuilder();
            elementos = datos.Count;

            result.AppendLine("Recorrido por niveles de la MinHeap:");

            if(heap == null || heap.Length != elementos)
{
                List<Proceso> list = new List<Proceso>();
                ShortesJobFirst(datos, list);
                heap = list.ToArray(); // Conversión a array y asignación
            }
            result.AppendLine(PorNiveles(heap, elementos));

            result.AppendLine("\nRecorrido por niveles de la MaxHeap:");

            if (maxHeap == null || maxHeap.Length != elementos)
            {
                // Por si no se ejecutó para tener maxHeap con datos
                List<Proceso> list = new List<Proceso>();
                PreemptivePriority(datos, list);
                maxHeap = list.ToArray(); // Conversión a array y asignación
            }
            result.AppendLine(PorNiveles(maxHeap, elementos));

            return result.ToString();
        }


        // Algoritmo Shortest Job First (SJF) usando MinHeap
        public void ShortesJobFirst(List<Proceso> datos, List<Proceso> collected)
        {
            //cantidad de procesos 
            elementos = datos.Count;
            //inicializacion de array con cantidad de elementos igual a datos
            heap = new Proceso[elementos];

            // Insertar todos los procesos en la heap
            InsertarEnHeap(datos, heap);

            // Reacomodar la heap con buildHeap
            BuildMinHeap();
            // Extraer elementos sin eliminarlos para que despues los usen los demas metodos
            for (int i = 0; i < elementos; i++)
            {
                collected.Add(heap[i]);
            }
        }

        // Algoritmo PreemptivePriority usando MaxHeap
        public void PreemptivePriority(List<Proceso> datos, List<Proceso> collected)
        {
            //cantidad de procesos 
            elementos = datos.Count;
            //inicializacion de array Max Heap
            maxHeap = new Proceso[elementos];

            // Insertar todos los procesos en la heap
            InsertarEnHeap(datos, maxHeap);

            // Reacomodar la heap con buildHeap
            BuildMaxHeap();
            // Extraer elementos y formar la lista final
            for (int i = 0; i < elementos; i++)
            {
                collected.Add(maxHeap[i]);
            }
        }

        // Métodos para MinHeap (basado en tiempo)
        private void BuildMinHeap()
        {
            for (int i = (elementos / 2) - 1; i >= 0; i--)
            {
                FiltradoAbajoMin(i, heap);
            }
        }

        private int TamañoHeap()
        {
            return elementos;
        }

        //insertar datos como viene, toma de parametro la lista de donde va a sacar la informacion
        //y un array donde insertarlo, APLICA PARA AMBAS HEAP

        private void InsertarEnHeap(List<Proceso> listaProcesos, Proceso[] array)
        {
            for (int i = 0; i < listaProcesos.Count; i++)
            { array[i] = listaProcesos[i]; }
        }
        


        // Heapify para MinHeap con array (tiempo) Barrido hacia abajo
        private void FiltradoAbajoMin(int indice, Proceso[]heap)
        {
            int menor = indice;
            while (true)
            {
                int hijoIzquierdo = 2 * indice + 1;
                int hijoDerecho = 2 * indice + 2;
                if (hijoIzquierdo < elementos && heap[hijoIzquierdo].tiempo < heap[menor].tiempo)
                {
                    menor = hijoIzquierdo;
                }
                if (hijoDerecho < elementos && heap[hijoDerecho].tiempo < heap[menor].tiempo)
                {
                    menor = hijoDerecho;
                }
                if (menor == indice)
                    break;
                Intercambiar(indice, menor, heap);
                indice = menor;
            }
        }

        // Métodos para MaxHeap (basado en prioridad)
        private void BuildMaxHeap()
        {
            for (int i = (elementos / 2) - 1; i >= 0; i--)
            {
                FiltradoAbajoMax(i, maxHeap);
            }
        }
        
        // filtrado hacia abajo para MaxHeap (prioridad)


        private void FiltradoAbajoMax(int indice, Proceso[] maxHeap)
        {
            elementos = maxHeap.Count();
            while (true)
            {
                int mayor = indice;
                int hijoIzquierdo = 2 * indice + 1;
                int hijoDerecho = 2 * indice + 2;

                if (hijoIzquierdo < elementos && maxHeap[hijoIzquierdo] != null && maxHeap[hijoIzquierdo].prioridad > maxHeap[mayor].prioridad)
                {
                    mayor = hijoIzquierdo;
                }

                if (hijoDerecho < elementos && maxHeap[hijoDerecho] != null && maxHeap[hijoDerecho].prioridad > maxHeap[mayor].prioridad)
                {
                    mayor = hijoDerecho;
                }

                if (mayor == indice) break;

                Intercambiar(indice, mayor, maxHeap);
                indice = mayor;
            }
        }


        //recorrido por niveles para la consulta 3
        public string PorNiveles(Proceso[] h, int cantElem)
        {
            // Donde se va a guardar el recorrido
            StringBuilder result = new StringBuilder();

            // Instancio la cola
            Cola<int> c = new Cola<int>();
            // Guardo el primer elemento de la heap
            if (cantElem > 0)
            {
                c.encolar(0);
                Console.WriteLine("Encolado el índice 0 al inicio.");
            }

            // Contador de nivel
            int nivel = 0;

            // Itero hasta que no hayan más datos
            while (!c.esVacia())
            {
                // Cantidad de nodos del nivel
                int nodos = c.cantidadElementos();
                // Indicador de nivel
                result.Append("Nivel " + nivel + ": ");

                for (int i = 0; i < nodos; i++)
                {
                    // Guardo el dato en el indice
                    int indice = c.desencolar();

                    // Para mostrar el dato actual
                    Proceso actual = h[indice];

                    // Añadir nombre del proceso a la línea actual
                    if (i > 0)
                    {
                        result.Append(", ");
                    }
                    result.Append(actual.nombre);

                    // Posicion hijos
                    int hijoIzquierdo = 2 * indice + 1;
                    int hijoDerecho = 2 * indice + 2;

                    // Ver si tienen hijos y depuración
                    if (hijoIzquierdo < cantElem)
                    {
                        c.encolar(hijoIzquierdo);
                        
                    }
                    if (hijoDerecho < cantElem)
                    {
                        c.encolar(hijoDerecho);
                        
                    }
                }
                result.AppendLine();
                nivel++;
            }

            return result.ToString();
        }


        // Método auxiliar para intercambiar elementos
        private void Intercambiar(int i, int j, Proceso[]h)
        {
            Proceso temp = h[i];
            h[i] = h[j];
            h[j] = temp;
        }
    }
}