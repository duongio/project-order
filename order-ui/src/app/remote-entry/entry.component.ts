import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
    selector: 'app-entry',
    standalone: false,
    templateUrl: './entry.component.html',
})
export class EntryComponent {
    constructor(
        private router: Router,
    ) { }

    onHistory() {
        this.router.navigate(['/payment-history']);
    }

    onReport() {
        this.router.navigate(['/report']);
    }
}