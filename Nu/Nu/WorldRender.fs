// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2020.

namespace Nu
open System
open Prime
open Nu

[<AutoOpen; ModuleBinding>]
module WorldRender =

    type World with

        static member internal getRenderer world =
            world.Subsystems.Renderer

        static member internal setRenderer renderer world =
            World.updateSubsystems (fun subsystems -> { subsystems with Renderer = renderer }) world

        static member internal updateRenderer updater world =
            World.setRenderer (updater (World.getRenderer world)) world

        /// Enqueue a rendering message to the world.
        static member enqueueRenderMessage (message : RenderMessage) world =
            world.Subsystems.Renderer.EnqueueMessage message
            world

        /// Enqueue multiple rendering messages to the world.
        static member enqueueRenderMessages (messages : RenderMessage seq) world =
            let renderer = World.getRenderer world
            for message in messages do renderer.EnqueueMessage message
            world
            
        /// Enqueue a layered message for rendering, bypassing enqueueRenderMessage for speed.
        static member enqueueRenderLayeredMessage (message : RenderLayeredMessage) world =
            world.Subsystems.Renderer.EnqueueLayeredMessage message
            world

        /// Enqueue multiple layered rendering messages to the world, bypassing enqueueRenderMessage for speed.
        static member enqueueRenderLayeredMessages (messages : RenderLayeredMessage seq) world =
            let renderer = World.getRenderer world
            for message in messages do renderer.EnqueueLayeredMessage message
            world

        /// Hint that a rendering asset package with the given name should be loaded. Should be
        /// used to avoid loading assets at inconvenient times (such as in the middle of game play!)
        [<FunctionBinding>]
        static member hintRenderPackageUse packageName world =
            let hintRenderPackageUseMessage = HintRenderPackageUseMessage packageName
            World.enqueueRenderMessage hintRenderPackageUseMessage world
            
        /// Hint that a rendering package should be unloaded since its assets will not be used
        /// again (or until specified via World.hintRenderPackageUse).
        [<FunctionBinding>]
        static member hintRenderPackageDisuse packageName world =
            let hintRenderPackageDisuseMessage = HintRenderPackageDisuseMessage packageName
            World.enqueueRenderMessage hintRenderPackageDisuseMessage world
            
        /// Send a message to the renderer to reload its rendering assets.
        [<FunctionBinding>]
        static member reloadRenderAssets world =
            let reloadRenderAssetsMessage = ReloadRenderAssetsMessage
            World.enqueueRenderMessage reloadRenderAssetsMessage world