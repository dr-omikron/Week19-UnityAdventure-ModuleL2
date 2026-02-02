using System.Collections;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.LoadScreen;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            DIContainer projectContainer = new DIContainer();
            ProjectContextRegistrations.Process(projectContainer);

            projectContainer.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(projectContainer));
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();
            SaveLoadDataProvidersService saveLoadDataProvidersService = container.Resolve<SaveLoadDataProvidersService>();

            loadingScreen.Show();

            yield return container.Resolve<ConfigsProviderService>().LoadAsync();

            yield return saveLoadDataProvidersService.CheckExistsSaving();

            loadingScreen.Hide();

            yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
        }

        
    }
}
