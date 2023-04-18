using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    internal class PackageController
    {
        public bool Insert(Packages packages)
        {
            return new PackageServices().Insert(packages);
        }
    }
}
