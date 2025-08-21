import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { LedgerAccountTypeComponent } from './ledger-account-type/ledger-account-type.component';
import { LedgerAccountComponent } from './ledger-account/ledger-account.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ledger-account-shell-page',
  imports: [ CommonModule, LedgerAccountTypeComponent, LedgerAccountComponent],
  templateUrl: './ledger-account-shell-page.component.html',
  styleUrls: ['./ledger-account-shell-page.component.scss']
})
export class LedgerAccountShellPageComponent implements OnInit, OnDestroy {
  selectedTypeId: string | null = null;
  private destroyed$ = new Subject<void>();

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntil(this.destroyed$))
      .subscribe(q => {
        this.selectedTypeId = q.get('typeId');
      });
  }

  onTypeChange(typeId: string) {
    this.selectedTypeId = typeId; 
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { typeId },
      queryParamsHandling: 'merge',
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
