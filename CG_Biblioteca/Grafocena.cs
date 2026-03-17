using System;
using System.Collections.Generic;

namespace CG_Biblioteca
{
  public static class Grafocena
  {
    public static void GrafocenaAtualizar(Objeto mundo, Dictionary<char, Objeto> grafoLista)
    {
      grafoLista.Clear();
      grafoLista = mundo.GrafocenaAtualizar(grafoLista);
    }

    public static Objeto GrafoCenaProximo(Objeto mundo, Objeto objetoSelecionado, Dictionary<char, Objeto> grafoLista)
    {
      GrafocenaAtualizar(mundo, grafoLista);
      var itGrafo = grafoLista.GetEnumerator();
      if (!itGrafo.MoveNext())
      {
        return mundo;
      }
      var primeiroEnumerado = itGrafo.Current.Value;
      if (!itGrafo.MoveNext())
      {
        return primeiroEnumerado;
      }
      var primeiroObjeto = itGrafo.Current.Value;
      if (objetoSelecionado == null)
      {
        objetoSelecionado = primeiroObjeto;
        return objetoSelecionado;
      }
      if (objetoSelecionado.Rotulo == '@')
      {
        objetoSelecionado = primeiroObjeto;
        return objetoSelecionado;
      }
      do
      {
        if (itGrafo.Current.Key == objetoSelecionado.Rotulo)
        {
          if (itGrafo.MoveNext())
            objetoSelecionado = itGrafo.Current.Value;
          else
            objetoSelecionado = primeiroObjeto;
          return objetoSelecionado;
        }
      } while (itGrafo.MoveNext());
      return primeiroObjeto;
    }

    public static void GrafoCenaImprimir(Objeto mundo, Dictionary<char, Objeto> grafoLista)
    {
      GrafocenaAtualizar(mundo, grafoLista);
      Console.WriteLine("__________________________________ \n");
      foreach (var par in grafoLista)
      {
        // Console.WriteLine($"Chave: {par.Key}, Valor: {par.Value}");
        Console.WriteLine($"Chave: {par.Key}");
      }
    }


  }
}