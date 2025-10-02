export interface ActionMenuOption {
      label: string;
      icon?: string; // ví dụ: '⚙️', '👨🏻‍💼', '⛔'
      color?: string; // ví dụ: 'red'
      disabled?: boolean;
      action?: () => void;
}