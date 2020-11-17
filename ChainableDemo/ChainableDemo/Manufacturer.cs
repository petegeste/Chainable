using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChainableDemo
{
    [DebuggerDisplay("Manufacturer {Name}")]
    public class Manufacturer : Chainable
    {
        public readonly string Name;
        public List<Vendor> Vendors;

        public Manufacturer(string name)
        {
            Name = name;
            Vendors = new List<Vendor>();
        }

        /// <summary>
        /// Creates an instance of the Manufacturer, or returns one from the chain
        /// </summary>
        /// <param name="name"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Manufacturer ChainedManufacturer(string name, Manufacturer other)
        {
            var res = other?.SearchChain<Manufacturer>(name);
            if (res == null)
            {
                res = new Manufacturer(name);
                other?.AddNodeToChain(res);
            }
            return res;
        }

        /// <summary>
        /// Adds a vendor to the vendor collection
        /// </summary>
        /// <param name="vendor"></param>
        public void AddVendor(Vendor vendor)
        {
            if (Vendors.Contains(vendor))
            {
                return;
            }
            Vendors.Add(vendor);
            vendor.AddManufacturer(this);
        }

        /// <summary>
        /// Determines if the manufacturer has the given vendor
        /// </summary>
        /// <returns>True if vendor is connected</returns>
        public bool HasVendor(Vendor vendor)
        {
            return Vendors.Contains(vendor);
        }

        public override string NodeId => Name;

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
