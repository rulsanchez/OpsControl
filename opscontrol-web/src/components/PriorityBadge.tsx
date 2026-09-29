type PriorityBadgeProps = {
    priority: string
}
const labels: Record<string, string> = {
    Low: 'Baja',
    Medium: 'Media',
    High: 'Alta',
    Critical: 'Crítica'
}
export default function PriorityBadge({ priority }: PriorityBadgeProps) {
    return (
        <span className={priority === 'Critical' ? 'priority-critical' : ''}>
            {labels[priority] ?? priority}
        </span>
    )
}