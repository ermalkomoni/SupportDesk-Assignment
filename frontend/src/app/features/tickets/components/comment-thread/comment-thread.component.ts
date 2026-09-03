import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

import { AddCommentRequest } from '../../models/ticket-requests.model';
import { TicketComment } from '../../models/ticket.model';

@Component({
  selector: 'sd-comment-thread',
  standalone: true,
  imports: [ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './comment-thread.component.html',
})
export class CommentThreadComponent {
  @Input({ required: true }) comments: TicketComment[] = [];
  @Input() set submitting(value: boolean) {
    this.isSubmitting = value;
    const controls = [this.authorName, this.body];

    for (const control of controls) {
      if (value) {
        control.disable({ emitEvent: false });
      } else {
        control.enable({ emitEvent: false });
      }
    }
  }

  get submitting(): boolean {
    return this.isSubmitting;
  }

  @Input() readOnly = false;
  @Output() readonly addComment = new EventEmitter<AddCommentRequest>();

  protected readonly authorName = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });
  protected readonly body = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });

  private isSubmitting = false;

  private readonly relativeTime = new Intl.RelativeTimeFormat(undefined, {
    numeric: 'auto',
  });

  protected submit(): void {
    if (!this.authorName.value.trim()) {
      this.authorName.setErrors({ required: true });
    }
    if (!this.body.value.trim()) {
      this.body.setErrors({ required: true });
    }

    if (this.authorName.invalid || this.body.invalid) {
      this.authorName.markAsTouched();
      this.body.markAsTouched();
      return;
    }

    this.addComment.emit({
      authorName: this.authorName.value.trim(),
      body: this.body.value.trim(),
    });
    this.body.reset();
  }

  protected relativeCreatedDate(value: string): string {
    const seconds = (Date.parse(value) - Date.now()) / 1000;
    const units: ReadonlyArray<[Intl.RelativeTimeFormatUnit, number]> = [
      ['year', 31_536_000],
      ['month', 2_592_000],
      ['week', 604_800],
      ['day', 86_400],
      ['hour', 3_600],
      ['minute', 60],
      ['second', 1],
    ];

    for (const [unit, secondsInUnit] of units) {
      if (Math.abs(seconds) >= secondsInUnit || unit === 'second') {
        return this.relativeTime.format(Math.round(seconds / secondsInUnit), unit);
      }
    }

    return 'just now';
  }
}
