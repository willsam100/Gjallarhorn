namespace Gjallarhorn.XamarinForms

open System
open System.Diagnostics
open System.Threading
open System.Threading.Tasks

/// Platform installation
module Platform =
    let private creation (typ : System.Type) =
        let sourceType = typedefof<Gjallarhorn.XamarinForms.RefTypeBindingTarget<_>>.MakeGenericType([|typ|])
        System.Activator.CreateInstance(sourceType) 

    /// Installs Xamarin Forms targets for binding into Gjallarhorn
    [<CompiledName("Install")>]
    let install () =        
        Gjallarhorn.Bindable.Binding.Implementation.installCreationFunction (fun _ -> creation typeof<obj>) creation

/// Xamarin Forms implementation of the basic application framework
module Framework =
    open Gjallarhorn
    open Gjallarhorn.Bindable    

    open Xamarin.Forms

    let runTask task = 
        task
        |> Async.AwaitIAsyncResult 
        |> Async.Ignore
        |> Async.Start

    let runOnUi f = 
        Action f |> Xamarin.Forms.Device.BeginInvokeOnMainThread 

    let runXmarinFormsApplication (core : Framework.ApplicationCore<'Model, 'Message>) (view: Page) =
        
        let render (createCtx : SynchronizationContext -> ObservableBindingSource<'Message>) = 
            view.BindingContext <- createCtx SynchronizationContext.Current
            1       

        let toApplicationSpecification : Framework.ApplicationSpecification<'Model,'Message> = 
            { Core = { Model = core.Model ; Init = core.Init ; Update = core.Update ; Binding = core.Binding } ; Render = render } 

        Gjallarhorn.Bindable.Framework.runApplication toApplicationSpecification 

    let changePage (navigate: Page -> Task) (core : Framework.ApplicationCore<'Model, 'Message>) (view: Page) =
        let changePageAndRun () =
            runXmarinFormsApplication core view |> ignore 
            view |> navigate |> runTask
            Debug.WriteLine <| sprintf "Galllarhorn XF:  page change requsted on UI thread"    
            
        Debug.WriteLine <| sprintf "Galllarhorn XF:  Changing page"    
        runOnUi changePageAndRun


    /// Default Xamarin Forms Application implementation
    type App(page) as self =
        inherit Application()    
        do         
            self.MainPage <- page

    type XamarinApplicationInfo<'Model,'Message> = 
        { 
            Core : Framework.ApplicationCore<'Model, 'Message>
            View : Page
        }
        with
            member this.CreateApp() =
                Platform.install ()
                this.Core.Init ()
                runXmarinFormsApplication this.Core this.View |> ignore
                App(this.View)                

    [<CompiledName("CreateApplicationInfo")>]
    /// Create the application core given a specific view
    let createApplicationInfo core view = { Core = core ; View = view }
