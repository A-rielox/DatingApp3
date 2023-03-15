import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
   selector: 'app-text-input',
   templateUrl: './text-input.component.html',
   styleUrls: ['./text-input.component.css'],
})
export class TextInputComponent implements ControlValueAccessor {
   @Input() label = '';
   @Input() type = 'text';

   // de esta forma tenemos acceso al control dentro DE ESTE componente ( cuando lo use dentro de las form desde las que lo voy a llamar )
   constructor(@Self() public ngControl: NgControl) {
      // this es el TextInputComponent
      this.ngControl.valueAccessor = this;
   }

   writeValue(obj: any): void {}
   registerOnChange(fn: any): void {}
   registerOnTouched(fn: any): void {}

   get control(): FormControl {
      return this.ngControl.control as FormControl;
   }
}
