export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>;
}

export class ApiError extends Error {
  constructor(
    public readonly status: number,
    public readonly title: string,
    public readonly detail: string,
    public readonly fieldErrors: Record<string, string[]> = {},
  ) {
    super(detail || title);
    this.name = 'ApiError';
  }

  errorsFor(field: string): string[] {
    const matchingKey = Object.keys(this.fieldErrors).find(
      (key) => key.toLowerCase() === field.toLowerCase(),
    );

    return matchingKey ? this.fieldErrors[matchingKey] : [];
  }
}
