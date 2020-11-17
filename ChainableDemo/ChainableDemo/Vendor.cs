using System.Collections.Generic;
using System.Diagnostics;

namespace ChainableDemo
{
    [DebuggerDisplay("Vendor {Name}")]
    public class Vendor : Chainable
    {
        public readonly string Name;
        public List<Manufacturer> Manufacturers;

        public Vendor(string name)
        {
            Name = name;
            Manufacturers = new List<Manufacturer>();
        }

        public static Vendor ChainedVendor(string name, Vendor other)
        {
            var res = other?.SearchChain<Vendor>(name);
            if (res == null)
            {
                res = new Vendor(name);
                other?.AddNodeToChain(res);
            }
            return res;
        }

        /// <summary>
        /// Adds a Manufacturer to the collection
        /// </summary>
        /// <param name="manufacturer"></param>
        public void AddManufacturer(Manufacturer manufacturer)
        {
            if (Manufacturers.Contains(manufacturer))
            {
                return;
            }
            Manufacturers.Add(manufacturer);
            manufacturer.AddVendor(this);
        }

        public override string NodeId => Name;

        public override void Dispose()
        {
            UnlinkFromChain();
        }
    }
}
