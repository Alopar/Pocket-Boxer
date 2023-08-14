using NaughtyAttributes;
using Services.SaveSystem;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class DependencyTest : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;

        [Inject("b")] private A _b;
        [Inject("c")] private A _c;
        //[Inject] private CCC _ccc;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            DependenciesContext.Bind<A>();
            DependenciesContext.Bind<A>("b").To<B>();
            DependenciesContext.Bind<A>("c").To<C>();
            DependenciesContext.Bind<CCC>().AsSingle();

            DependenciesContext.Inject(this);
        }

        private void Start()
        {
            _b.Print();
            _c.Print();
        }

        private void Update()
        {
            //var loadData = _saveService.Load<CurrencySaveData>();
            //print(loadData.Money);
        }

        [Button("CreateCCC")]
        public void CreateCCC()
        {
            var ccc = DependenciesContext.Get<CCC>();
            print(ccc.Index);
        }
        #endregion
    }

    public abstract class A
    {
        public abstract void Print();
    }

    public class B : A
    {
        public override void Print()
        {
            Debug.Log("B");
        }
    }

    public class C : A
    {
        public override void Print()
        {
            Debug.Log("C");
        }
    }

    public class CCC
    {
        private static int _counter = 0;

        private int _index = 0;
        public int Index => _index;

        public CCC()
        {
            _index = _counter++;
        }
    }
}
