// ============================================================
// FILE: src/hooks/useFetch.js
//
// WHY A CUSTOM HOOK?
//   Reusable loading/error/data state pattern.
//   Prevents copy-pasting useState + useEffect in every page.
//
// ── ROOT CAUSE OF THE INFINITE LOOP ─────────────────────────
// The bug was this function signature:
//
//   export function useFetch(serviceCall, deps = [])
//                                              ^^^
// Every time the component renders, JavaScript evaluates
// deps = [] and creates a BRAND NEW array object in memory.
//
// Even though the array looks the same, in JS:
//   [] === []  →  FALSE  (different references in memory)
//
// So useCallback saw its dependency change every render:
//   render → new [] → useCallback re-creates refetch
//   → useEffect sees new refetch → calls refetch()
//   → setLoading(true) → setState → re-render
//   → new [] → useCallback re-creates... ∞ INFINITE LOOP
//
// ── THE FIX ──────────────────────────────────────────────────
// Remove the deps parameter entirely.
// Use useRef to hold serviceCall — refs never trigger re-renders.
// useCallback gets a truly empty [] that never changes.
// useEffect runs ONCE on mount, and whenever you call refetch().
// ============================================================

import { useState, useEffect, useRef, useCallback } from 'react';

export function useFetch(serviceCall) {

  const [data,    setData]    = useState([]);
  const [loading, setLoading] = useState(true);
  const [error,   setError]   = useState(null);

  // ── WHY useRef for serviceCall? ───────────────────────────
  // Storing serviceCall in a ref means:
  //   1. We always call the LATEST version of the function
  //   2. Changing the ref does NOT trigger a re-render
  //   3. useCallback can safely use [] as its dependency array
  //      because it reads from the ref, not from the argument
  const serviceCallRef = useRef(serviceCall);

  // Keep the ref up to date on every render (no re-render triggered)
  serviceCallRef.current = serviceCall;

  // ── refetch: stable function that never changes reference ──
  // useCallback with [] means this function is created ONCE.
  // It reads from serviceCallRef.current so it always calls
  // the latest version of serviceCall without needing deps.
  const refetch = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await serviceCallRef.current();
      setData(response.data);
    } catch (err) {
      setError(err.response?.data?.message || 'Something went wrong');
    } finally {
      setLoading(false);
    }
  }, []); // ← [] is safe here because we use the ref above

  // ── useEffect: runs ONCE on mount ─────────────────────────
  // refetch is stable (never changes) so this effect fires once.
  // After that, only manual refetch() calls trigger a reload.
  useEffect(() => {
    refetch();
  }, [refetch]);

  return { data, loading, error, setData, refetch };
}