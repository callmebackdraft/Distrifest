﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using DataHandling;
using Models;
using System.Data;

namespace Repositories
{
    public class DeliveryLineRepository : IDeliveryLineRepository
    {
        IDeliveryLineContext DeLictx;

        public DeliveryLineRepository()
        {
            DeLictx = new DeliveryLineContext();
        }
        public List<DeliveryLine> GetAllDeliveryLines()
        {
            List<DeliveryLine> result = new List<DeliveryLine>();
            foreach(DataRow dr in DeLictx.GetAllDeliveryLines().Rows)
            {
                result.Add(DataRowToDeliveryLine(dr));
            }
            return result;
        }

        public List<DeliveryLine> GetAllDeliveryLinesForDelivery(int _deliveryID)
        {
            List<DeliveryLine> result = new List<DeliveryLine>();
            foreach (DataRow dr in DeLictx.GetAllDeliveryLinesForDelivery(_deliveryID).Rows)
            {
                result.Add(DataRowToDeliveryLine(dr));
            }
            return result;
        }

        public void SaveAllDeliveryLines(Delivery _delivery)
        {
            IProductRepository ProdRepo = new ProductRepository();
            DeLictx.SaveAllDeliveryLines(_delivery);
            foreach(DeliveryLine _ol in _delivery.Products)
            {
                ProdRepo.UpdateAmountInStock(_ol.Product,_ol.Amount);
            }
        }

        public bool SaveDeliveryLine(int _deliveryID, DeliveryLine _deliveryLine)
        {
            IProductRepository ProdRepo = new ProductRepository();
            ProdRepo.UpdateAmountInStock(_deliveryLine.Product, _deliveryLine.Amount);
            return DeLictx.SaveDeliveryLine(_deliveryID, _deliveryLine);
        }

        private DeliveryLine DataRowToDeliveryLine(DataRow _dataRow)
        {
            return new DeliveryLine(new ProductRepository().GetProductByID(Convert.ToInt16(_dataRow.Field<decimal>("ProductID"))), Convert.ToInt16(_dataRow.Field<decimal>("Amount")));
        }
    }
}
