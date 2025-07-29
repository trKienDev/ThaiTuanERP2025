import { NgModule } from '@angular/core';
import { NgIconsModule } from '@ng-icons/core';
import { heroEye, heroEyeSlash } from '@ng-icons/heroicons/outline';

@NgModule({
  imports: [NgIconsModule.withIcons({ heroEye, heroEyeSlash })],
  exports: [NgIconsModule],
})
export class HeroiconsModule {}