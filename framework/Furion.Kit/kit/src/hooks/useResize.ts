import { RefObject, useEffect } from "react";

function useResize<T extends HTMLElement>(
  eleRef: RefObject<T>,
  callback: (contentRect: DOMRectReadOnly) => void
) {
  useEffect(() => {
    const element = eleRef.current;
    const observer = new ResizeObserver((entries) => {
      for (let entry of entries) {
        callback(entry.contentRect);
      }
    });

    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element && observer) {
        observer.unobserve(element);
      }
    };
  }, [callback, eleRef]);
}

export default useResize;
