#### 0.0.1-beta - May 1th 2016
* Initial publish 
* API will still change

#### 0.0.2-beta - May 18th 2016
* Corrected potential blocking condition in SignalManager with Mutable.create
* Converted commands to be IObservable<'a> but not ISignal<'a>.  This fits better with the conceptual notion of a command.

#### 0.0.3-beta - June 1st 2016
* Corrected issue where edit bindings were being created as typed ISignal<obj>, not ISignal<'a>

#### 0.0.4-beta - June 13th 2016
* Overhauled binding API to make things more clear.

#### 0.0.5-beta - December 2nd 2016
* Reworked notification plumbing to be less surprising
* Added initial framework for building applications directly
* Added component model to framework

#### 0.0.6-beta - December 8th 2016
* Improved collection handling performance via internal optimizations
 
#### 0.0.7-beta - December 12th 2016
* Added new Signal.toFunction and Signal.mapFunction members
* Added ObservableSource<'a> to ease working with observables
* Improved samples
 
#### 0.0.8-beta - December 14th 2016
* Added new thread safe and async mutable interfaces and core types
* Added generator functions in Mutable module for async and threadsafe mutables
* Renamed State to AsyncMutable, cleaned up API

 
