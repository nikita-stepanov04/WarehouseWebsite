import {OnInit, Input, Directive} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { FormHelperService } from '../../forms/form-helper.service';

@Directive()
export abstract class BaseInputComponent implements OnInit {
  @Input() label: string = '';
  @Input() controlName: string = '';
  @Input() parentForm!: FormGroup;

  control!: FormControl;

  constructor(public fh: FormHelperService) {}

  ngOnInit(): void {
    this.control = this.parentForm.get(this.controlName) as FormControl;
  }
}
