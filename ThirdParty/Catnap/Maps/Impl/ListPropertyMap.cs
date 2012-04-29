using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Catnap.Find;

namespace Catnap.Maps.Impl
{
    public class ListPropertyMap<TEntity, TListMember> : BasePropertyMap<TEntity, IEnumerable<TListMember>>, IListPropertyMap<TEntity>
        where TEntity : class, IEntity, new()
        where TListMember : class, IEntity, new()
    {
        private readonly Expression<Func<TListMember, bool>> filter;
        private IEntityMap listMap;

        public ListPropertyMap(Expression<Func<TEntity, IEnumerable<TListMember>>> property, bool isLazy, bool cascadeSaves, bool cascadeDeletes) 
            : this(property, isLazy, cascadeSaves, cascadeDeletes, null) { }

        public ListPropertyMap(Expression<Func<TEntity, IEnumerable<TListMember>>> property, bool isLazy)
            : this(property, isLazy, true, true, null) { }

        public ListPropertyMap(Expression<Func<TEntity, IEnumerable<TListMember>>> property, bool isLazy, bool cascadeSaves, bool cascadeDeletes, Expression<Func<TListMember, bool>> filter) 
            : base(property)
        {
            this.filter = filter;
            IsLazy = isLazy;
            WillCascadeSaves = cascadeSaves;
            WillCascadeDeletes = cascadeDeletes;
        }

        public bool IsLazy { get; private set; }
        public bool WillCascadeSaves { get; private set; }
        public bool WillCascadeDeletes { get; private set; }

        public void Cascade(ISession session, TEntity parent)
        {
            var existingList = Load(session, parent);
            var list = (IEnumerable<TListMember>)getter.Invoke(parent, null);
            var itemsToDelete = existingList.Except(list, new EntityEqualityComaparer<TListMember>());
            CascadeDeletes(session, itemsToDelete);
            CascadeSaves(session, parent, list);
        }

        public Type ItemTpye
        {
            get { return typeof(TListMember); }
        }

        public void SetListMap(IEntityMap map)
        {
            listMap = map;
        }

        private void CascadeSaves(ISession session, TEntity parent, IEnumerable<TListMember> list)
        {
            if (!WillCascadeSaves)
            {
                return;
            }
            foreach (var item in list)
            {
                session.SaveOrUpdate(item, parent.Id);
            }
        }

        private void CascadeDeletes(ISession session, IEnumerable<TListMember> itemsToDelete)
        {
            if (!WillCascadeDeletes)
            {
                return;
            }
            foreach (var item in itemsToDelete)
            {
                session.Delete<TListMember>(item.Id);
            }
        }

        public IList<TListMember> Load(ISession session, TEntity parent)
        {
            var builder = new FindCommandBuilder<TListMember>()
                .AddCondition(listMap.ParentColumnName, "=", parent.Id);
            if (filter != null)
            {
                builder.AddCondition(filter);
            }
            return session.List<TListMember>(builder.Build());
        }

        protected override void InnerSetValue(TEntity instance, object value, ISession session)
        {
            if (IsLazy)
            {
                var proxy = new LazyList<TListMember>(() => Load(session, instance));
                setter.Invoke(instance, new[] { proxy });
            }
            else
            {
                base.InnerSetValue(instance, Load(session, instance), session);
            }
        }
    }
}