using Fluxor;
using Fluxor.Persist.Middleware;
using Microsoft.AspNetCore.Components;

namespace BlazorSSR.Pages;

public abstract class BaseComponent : ComponentBase, IAsyncDisposable
{
    [Inject] public IActionSubscriber ActionSubscriber { get; private set; }

    [Inject] public IDispatcher Dispatcher { get; private set; }


    protected override Task OnInitializedAsync()
    {
        ActionSubscriber.SubscribeToAction<InitializePersistMiddlewareResultSuccessAction>(this, async result =>
        {
            // we now have state, we can re-render to reflect binding changes
            await InvokeAsync(() => StateHasChanged());
        });

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        ActionSubscriber.UnsubscribeFromAllActions(this);

        return ValueTask.CompletedTask;
    }
}