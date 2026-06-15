"use client"

import { useRef } from 'react'
import { Provider } from 'react-redux'
import { makeStore, AppStore } from '@/lib/store'
import AuthBootstrap from "@/app/AuthBootstrap";

const StoreProvider = ({children}: { children: React.ReactNode }) => {
    const storeRef = useRef<AppStore | null>(null)
    if (storeRef.current === null) {
        // Create the store instance the first time this renders
        storeRef.current = makeStore();
    }

    return (
        <Provider store={storeRef.current}>
            <AuthBootstrap />
            {children}
        </Provider>
    );
}

export default StoreProvider;