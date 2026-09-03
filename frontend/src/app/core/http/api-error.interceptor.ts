import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

import { ApiError, ProblemDetails } from './problem-details.model';

export const apiErrorInterceptor: HttpInterceptorFn = (request, next) =>
  next(request).pipe(
    catchError((error: unknown) => {
      if (!(error instanceof HttpErrorResponse) || error.status === 0) {
        return throwError(
          () =>
            new ApiError(
              0,
              'Unable to reach the server',
              'An unexpected network error occurred. Please try again.',
            ),
        );
      }

      const problemDetails = isProblemDetails(error.error) ? error.error : undefined;
      const apiError = new ApiError(
        problemDetails?.status ?? error.status,
        problemDetails?.title ?? 'Request failed',
        problemDetails?.detail ?? error.message,
        problemDetails?.errors ?? {},
      );

      return throwError(() => apiError);
    }),
  );

function isProblemDetails(value: unknown): value is ProblemDetails {
  return typeof value === 'object' && value !== null;
}
