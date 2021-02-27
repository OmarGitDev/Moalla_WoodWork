using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using TSD_DAL.TSD_EDMX;

namespace TSD_BLL
{
    public class GenericBLL<TEntity> where TEntity : class
    {
        static string errorMessage = string.Empty;
        public static TEntity GetById(object id)
        {
            TSD_Gestion_CommercialeEntities context = new TSD_Gestion_CommercialeEntities();
            return context.Set<TEntity>().Find(id);
        }
        public static List<TEntity> GetAll()
        {
            TSD_Gestion_CommercialeEntities context = new TSD_Gestion_CommercialeEntities();
            return context.Set<TEntity>().ToList();
        }
        public static void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                TSD_Gestion_CommercialeEntities context = new TSD_Gestion_CommercialeEntities();
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public static void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                TSD_Gestion_CommercialeEntities context = new TSD_Gestion_CommercialeEntities();
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(errorMessage, dbEx);
            }
        }

        public static void Delete(object id)
        {
            try
            {
                TSD_Gestion_CommercialeEntities context = new TSD_Gestion_CommercialeEntities();
                var entity = context.Set<TEntity>().Find(id);
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }
    }

}