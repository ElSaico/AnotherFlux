using System;
using System.Collections.Generic;
using FluxShared;

namespace AnotherFlux.Models
{
  internal class MainFormModel
  {
    public byte[][] LecSize { get; }
    public Dictionary<string, int> RecTypes { get; }
    public string[] LocComType { get; }
    public string[] OWComType { get; }
    public string[] IfOp { get; }
    public string[] ObjFunc { get; }
    public string[] Animations { get; }

    public static void InitializeGlobalShared()
    {
      GlobalShared.PostStatus = sStatus =>
      {
        Console.Out.WriteLine(sStatus);
        //MainStatus.Text = $"{sStatus}  ({DateTime.Now.ToLongTimeString()})";
        //Update();
      };
      //GetStrFromGroup = getStrFromGroup;
    }
  }
}