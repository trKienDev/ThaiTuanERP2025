import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { FollowingOutgoingPaymentComponent } from "./following-outgoing-payment/following-outgoing-payment.component";
import { OutgoingPaymentRequestComponent } from "./outgoing-payment-request/outgoing-payment-request.component";

@Component({
      selector: 'outgoing-payment-shell-page',
      standalone: true,
      templateUrl: './outgoing-payment-shell-page.component.html',
      imports: [ CommonModule, KitShellTabsComponent]
})
export class OutgoingPaymentShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'following-outgoing-payments', label: 'Khoản tiền ra', component: FollowingOutgoingPaymentComponent },
            { id: 'outgoing-payment-request', label: 'Khoản tiền mới', component: OutgoingPaymentRequestComponent, hidden: true }
      ]
}