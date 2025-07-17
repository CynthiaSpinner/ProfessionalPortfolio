import { useState, useEffect, useCallback, useRef } from 'react';

export const usePortfolioData = (fetchFunction, dependencies = [], fallbackData = null) => {
  const [data, setData] = useState(fallbackData);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const isMountedRef = useRef(true);

  const fetchData = useCallback(async () => {
    if (!isMountedRef.current) return;
    
    try {
      setLoading(true);
      setError(null);
      const result = await fetchFunction();
      if (isMountedRef.current) {
        setData(result);
      }
    } catch (err) {
      console.error("Error fetching data:", err);
      if (isMountedRef.current) {
        setError(err);
      }
    } finally {
      if (isMountedRef.current) {
        setLoading(false);
      }
    }
  }, [fetchFunction]);

  useEffect(() => {
    isMountedRef.current = true;
    fetchData();
    
    return () => {
      isMountedRef.current = false;
    };
  }, [fetchData]); // Only depend on fetchData

  // DISABLED: Auto-retry on error - causing infinite loops
  // useEffect(() => {
  //   // Completely disable retries for any network-related errors
  //   if (error && retryCount < maxRetries && isMountedRef.current) {
  //     const isNetworkError = 
  //       error.message?.includes('Network Error') || 
  //       error.code?.includes('ERR_NETWORK') ||
  //       error.code?.includes('ERR_INSUFFICIENT_RESOURCES') ||
  //       error.message?.includes('timeout') ||
  //       error.message?.includes('Failed to fetch');
  //     
  //     if (!isNetworkError) {
  //       const timeoutId = setTimeout(() => {
  //         fetchData();
  //       }, Math.pow(2, retryCount) * 1000); // Exponential backoff: 1s, 2s, 4s
  //       
  //       return () => clearTimeout(timeoutId);
  //     } else {
  //       console.log('Skipping retry for network error:', error.message || error.code);
  //     }
  //   }
  // }, [error, retryCount, fetchData]);

  const refetch = useCallback(() => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, refetch };
}; 